;-----------------------------------------------------------------------------;
; Fixed-point FFT routines for megaAVRs                        (C)ChaN, 2005
;-----------------------------------------------------------------------------;
;
; void fft_input (const int16_t *array_src, complex_t *array_bfly);
; void fft_execute (complex_t *array_bfly);
; void fft_output (complex_t *array_bfly, uint16_t *array_dst);
;
;  <array_src>: Wave form to be processed.
;  <array_bfly>: Complex array for butterfly operations.
;  <array_dst>: Spectrum output buffer.
;
; These functions must be called in sequence to do a DFT in FFT algorithm.
; fft_input() fills the complex array with a wave form to prepare butterfly
; operations. A hamming window is applied at the same time.
; fft_execute() executes the butterfly operations.
; fft_output() re-orders the results, converts the complex spectrum into
; scalar spectrum and output it in linear scale.
;
; The number of points FFT_N is defined in "ffft.h" and the value can be
; power of 2 in range of 64 - 1024.
;
;----------------------------------------------------------------------------;
; 16bit fixed-point FFT performance with MegaAVRs
; (Running at 16MHz/internal SRAM)
;
;  Points:   Input, Execute,  Output,    Total:  Throughput
;   64pts:   .17ms,   2.0ms,   1.2ms,    3.4ms:   19.0kpps
;  128pts:   .33ms,   4.6ms,   2.4ms,    7.3ms:   17.5kpps
;  256pts:   .66ms,  10.4ms,   4.9ms,   15.9ms:   16.1kpps
;  512pts:   1.3ms,  23.2ms,   9.7ms,   34.2ms:   14.9kpps
; 1024pts:   2.7ms,  51.7ms,  19.4ms,   73.7ms:   13.9kpps
;----------------------------------------------------------------------------;


.nolist
#define FFFT_ASM
#include "ffft.h"
.list

#define FFT_B_64 6

;----------------------------------------------------------------------------;
; Constant Tables

.global tbl_window_64
tbl_window_64:	; tbl_window[] = ... (This is a Hamming window)
#if FFT_N_64 == 64
	.dc.w	2621, 2693, 2910, 3270, 3768, 4401, 5161, 6042, 7036, 8132, 9320, 10588, 11926, 13318, 14753, 16216
	.dc.w	17694, 19171, 20634, 22069, 23462, 24799, 26068, 27256, 28352, 29345, 30226, 30987, 31619, 32117, 32477, 32694
	.dc.w	32766, 32694, 32477, 32117, 31619, 30987, 30226, 29345, 28352, 27256, 26068, 24799, 23462, 22069, 20634, 19171
	.dc.w	17694, 16216, 14753, 13318, 11926, 10588, 9320, 8132, 7036, 6042, 5161, 4401, 3768, 3270, 2910, 2693
#endif


tbl_cos_sin:	; Table of {cos(x),sin(x)}, (0 <= x < pi, in FFT_N/2 steps)
#if FFT_N_64 == 64
	.dc.w	32767, 0, 32609, 3211, 32137, 6392, 31356, 9511, 30272, 12539, 28897, 15446, 27244, 18204, 25329, 20787
	.dc.w	23169, 23169, 20787, 25329, 18204, 27244, 15446, 28897, 12539, 30272, 9511, 31356, 6392, 32137, 3211, 32609
	.dc.w	0, 32766, -3211, 32609, -6392, 32137, -9511, 31356, -12539, 30272, -15446, 28897, -18204, 27244, -20787, 25329
	.dc.w	-23169, 23169, -25329, 20787, -27244, 18204, -28897, 15446, -30272, 12539, -31356, 9511, -32137, 6392, -32609, 3211
#endif



tbl_bitrev:		; tbl_bitrev[] = ...
#if FFT_N_64 == 64
#ifdef INPUT_IQ
	.dc.w	1*4, 33*4, 17*4, 49*4, 9*4, 41*4, 25*4, 57*4, 5*4, 37*4, 21*4, 53*4, 13*4, 45*4, 29*4, 61*4
	.dc.w	3*4, 35*4, 19*4, 51*4, 11*4, 43*4, 27*4, 59*4, 7*4, 39*4, 23*4, 55*4, 15*4, 47*4, 31*4, 63*4
#endif
	.dc.w	0*4, 32*4, 16*4, 48*4, 8*4, 40*4, 24*4, 56*4, 4*4, 36*4, 20*4, 52*4, 12*4, 44*4, 28*4, 60*4
	.dc.w	2*4, 34*4, 18*4, 50*4, 10*4, 42*4, 26*4, 58*4, 6*4, 38*4, 22*4, 54*4, 14*4, 46*4, 30*4, 62*4
#endif



;----------------------------------------------------------------------------;
#ifndef INPUT_NOISE
.global fft_input_64
.func fft_input_64
fft_input_64:
	pushw	T2H,T2L
	pushw	AH,AL
	pushw	YH,YL

	movw	XL, EL				;X = array_src;
	movw	YL, DL				;Y = array_bfly;
	clr	EH				;Zero
	ldiw	ZH,ZL, tbl_window_64		;Z = &tbl_window[0];
	ldiw	AH,AL, FFT_N_64			;A = FFT_N;
1:	lpmw	BH,BL, Z+			;B = *Z++; (window)
	ldw	CH,CL, X+			;C = *X++; (I-axis)
	FMULS16	DH,DL,T2H,T2L, BH,BL, CH,CL	;D = B * C;
	stw	Y+, DH,DL			;*Y++ = D;
#ifdef INPUT_IQ
	ldw	CH,CL, X+			;C = *X++; (Q-axis)
	FMULS16	DH,DL,T2H,T2L, BH,BL, CH,CL	;D = B * C;
#endif
	stw	Y+, DH,DL			;*Y++ = D;
	subiw	AH,AL, 1			;while(--A)
	brne	1b				;/

	popw	YH,YL
	popw	AH,AL
	popw	T2H,T2L
	clr	r1
	ret
.endfunc
#endif	/* INPUT_NOISE */



;----------------------------------------------------------------------------;
.global fft_execute_64
.func fft_execute_64
fft_execute_64:
	pushw	T2H,T2L
	pushw	T4H,T4L
	pushw	T6H,T6L
	pushw	T8H,T8L
	pushw	T10H,T10L
	pushw	T12H,T12L
	pushw	T14H,T14L
	pushw	AH,AL
	pushw	YH,YL

	movw	ZL, EL				;Z = array_bfly;
	ldiw	EH,EL, 1			;E = 1;
	ldiw	XH,XL, FFT_N_64/2			;X = FFT_N/2;
1:	ldi	AL, 4				;T12 = E; (angular speed)
	mul	EL, AL				;
	movw	T12L, T0L			;
	mul	EH, AL				;
	add	T12H, T0L			;/
	movw	T14L, EL			;T14 = E;
	pushw	EH,EL
	movw	YL, ZL				;Z = &array_bfly[0];
	mul	XL, AL				;Y = &array_bfly[X];
	addw	YH,YL, T0H,T0L			;
	mul	XH, AL				;
	add	YH, T0L				;/
	pushw	ZH,ZL
2:	clrw	T10H,T10L			;T10 = 0 (angle)
	clr	EH				;Zero reg.
3:	lddw	AH,AL, Z+0			;A = *Z - *Y; *Z++ += *Y;
	asrw	AH,AL				;
	lddw	DH,DL, Y+0			;
	asrw	DH,DL				;
	movw	CL, AL				;
	subw	AH,AL, DH,DL			;
	addw	CH,CL, DH,DL			;
	stw	Z+, CH,CL			;/
	lddw	BH,BL, Z+0			;B = *Z - *Y; *Z++ += *Y;
	asrw	BH,BL				;
	lddw	DH,DL, Y+2			;
	asrw	DH,DL				;
	movw	CL, BL				;
	subw	BH,BL, DH,DL			;
	addw	CH,CL, DH,DL			;
	stw	Z+, CH,CL			;/
	movw	T0L, ZL
	ldiw	ZH,ZL, tbl_cos_sin		;C = cos(T10); D = sin(T10);
	addw	ZH,ZL, T10H,T10L		;
	lpmw	CH,CL, Z+			;
	lpmw	DH,DL, Z+			;/
	movw	ZL, T0L
	FMULS16	T4H,T4L,T2H,T2L, AH,AL, CH,CL	;*Y++ = A * C + B * D;
	FMULS16	T8H,T8L,T6H,T6L, BH,BL, DH,DL	;
	addd	T4H,T4L,T2H,T2L, T8H,T8L,T6H,T6L;
	stw	Y+, T4H,T4L			;/
	FMULS16	T4H,T4L,T2H,T2L, BH,BL, CH,CL 	;*Y++ = B * C - A * D;
	FMULS16	T8H,T8L,T6H,T6L, AH,AL, DH,DL 	;
	subd	T4H,T4L,T2H,T2L, T8H,T8L,T6H,T6L;
	stw	Y+, T4H,T4L			;/
	addw	T10H,T10L, T12H,T12L		;T10 += T12; (next angle)
#if FFT_N_64 >= 128
	sbrs	T10H, FFT_B_64 - 7			;while(T10 < pi)
#else
	sbrs	T10L, FFT_B_64 + 1
#endif
	rjmp	3b				;/
	ldi	AL, 4				;Y += X; Z += X; (skip split segment)
	mul	XL, AL
	addw	YH,YL, T0H,T0L			;
	addw	ZH,ZL, T0H,T0L			;
	mul	XH, AL				;
	add	YH, T0L				;
	add	ZH, T0L				;/
	ldi	EL, 1				;while(--T14)
	subw	T14H,T14L, EH,EL		;
	rjne	2b				;/
	popw	ZH,ZL
	popw	EH,EL
	lslw	EH,EL				;E *= 2;
	lsrw	XH,XL				;while(X /= 2)
	adiw	XL, 0				;
	rjne	1b				;/

	popw	YH,YL
	popw	AH,AL
	popw	T14H,T14L
	popw	T12H,T12L
	popw	T10H,T10L
	popw	T8H,T8L
	popw	T6H,T6L
	popw	T4H,T4L
	popw	T2H,T2L
;	clr	r1
	ret
.endfunc



;----------------------------------------------------------------------------;
.global fft_output_64
.func fft_output_64
fft_output_64:
	pushw	T2H,T2L
	pushw	T4H,T4L
	pushw	T6H,T6L
	pushw	T8H,T8L
	pushw	T10H,T10L
	pushw	AH,AL
	pushw	YH,YL

	movw	T10L, EL			;T10 = array_bfly;
	movw	YL, DL				;Y = array_output;
	ldiw	ZH,ZL, tbl_bitrev		;Z = tbl_bitrev;
	clr	EH				;Zero
#ifdef INPUT_IQ
	ldiw	AH,AL, FFT_N_64			;A = FFT_N; (plus/minus)
#else
	ldiw	AH,AL, FFT_N_64 / 2		;A = FFT_N / 2; (plus only)
#endif
1:	lpmw	XH,XL, Z+			;X = *Z++;
	addw	XH,XL, T10H,T10L		;X += array_bfly;
	ldw	BH,BL, X+			;B = *X++;
	ldw	CH,CL, X+			;C = *X++;
	FMULS16	T4H,T4L,T2H,T2L, BH,BL, BH,BL	;T4:T2 = B * B;
	FMULS16	T8H,T8L,T6H,T6L, CH,CL, CH,CL	;T8:T6 = C * C;
	addd	T4H,T4L,T2H,T2L, T8H,T8L,T6H,T6L;T4:T2 += T8:T6;
	SQRT32					;B = sqrt(T4:T2);
	stw	Y+, BH,BL			;*Y++ = B;
	subiw	AH,AL, 1			;while(--A)
	rjne	1b				;/

	popw	YH,YL
	popw	AH,AL
	popw	T10H,T10L
	popw	T8H,T8L
	popw	T6H,T6L
	popw	T4H,T4L
	popw	T2H,T2L
	clr	r1
	ret
.endfunc



;----------------------------------------------------------------------------;
.global fmuls_f_64
.func fmuls_f_64
fmuls_f_64:
	movw	CL, EL				;C = E;
	clr	EH	;Zero
	FMULS16	ZH,ZL,XH,XL, CH,CL, DH,DL	;Z:X = C * D;
	movw	EL, ZL
	clr	r1
	ret
.endfunc

