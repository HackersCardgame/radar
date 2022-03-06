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

#define FFT_B_128 7

;----------------------------------------------------------------------------;
; Constant Tables

.global tbl_window_128
tbl_window_128:	; tbl_window[] = ... (This is a Hamming window)
#if FFT_N_128 == 128
	.dc.w	2621, 2639, 2693, 2784, 2910, 3073, 3270, 3502, 3768, 4068, 4401, 4765, 5161, 5587, 6042, 6525
	.dc.w	7036, 7571, 8132, 8715, 9320, 9945, 10588, 11249, 11926, 12616, 13318, 14031, 14753, 15482, 16216, 16954
	.dc.w	17694, 18433, 19171, 19905, 20634, 21356, 22069, 22772, 23462, 24138, 24799, 25443, 26068, 26673, 27256, 27816
	.dc.w	28352, 28862, 29345, 29800, 30226, 30622, 30987, 31319, 31619, 31885, 32117, 32315, 32477, 32603, 32694, 32748
	.dc.w	32766, 32748, 32694, 32603, 32477, 32315, 32117, 31885, 31619, 31319, 30987, 30622, 30226, 29800, 29345, 28862
	.dc.w	28352, 27816, 27256, 26673, 26068, 25443, 24799, 24138, 23462, 22772, 22069, 21356, 20634, 19905, 19171, 18433
	.dc.w	17694, 16954, 16216, 15482, 14753, 14031, 13318, 12616, 11926, 11249, 10588, 9945, 9320, 8715, 8132, 7571
	.dc.w	7036, 6526, 6042, 5587, 5161, 4765, 4401, 4068, 3768, 3502, 3270, 3073, 2910, 2784, 2693, 2639
#endif


tbl_cos_sin:	; Table of {cos(x),sin(x)}, (0 <= x < pi, in FFT_N/2 steps)
#if FFT_N_128 == 128
	.dc.w	32767, 0, 32727, 1607, 32609, 3211, 32412, 4807, 32137, 6392, 31785, 7961, 31356, 9511, 30851, 11038
	.dc.w	30272, 12539, 29621, 14009, 28897, 15446, 28105, 16845, 27244, 18204, 26318, 19519, 25329, 20787, 24278, 22004
	.dc.w	23169, 23169, 22004, 24278, 20787, 25329, 19519, 26318, 18204, 27244, 16845, 28105, 15446, 28897, 14009, 29621
	.dc.w	12539, 30272, 11038, 30851, 9511, 31356, 7961, 31785, 6392, 32137, 4807, 32412, 3211, 32609, 1607, 32727
	.dc.w	0, 32766, -1607, 32727, -3211, 32609, -4807, 32412, -6392, 32137, -7961, 31785, -9511, 31356, -11038, 30851
	.dc.w	-12539, 30272, -14009, 29621, -15446, 28897, -16845, 28105, -18204, 27244, -19519, 26318, -20787, 25329, -22004, 24278
	.dc.w	-23169, 23169, -24278, 22005, -25329, 20787, -26318, 19519, -27244, 18204, -28105, 16845, -28897, 15446, -29620, 14009
	.dc.w	-30272, 12539, -30851, 11038, -31356, 9511, -31784, 7961, -32137, 6392, -32412, 4807, -32609, 3211, -32727, 1607
#endif



tbl_bitrev:		; tbl_bitrev[] = ...
#if FFT_N_128 == 128
#ifdef INPUT_IQ
	.dc.w	1*4, 65*4, 33*4, 97*4, 17*4, 81*4, 49*4, 113*4, 9*4, 73*4, 41*4, 105*4, 25*4, 89*4, 57*4, 121*4
	.dc.w	5*4, 69*4, 37*4, 101*4, 21*4, 85*4, 53*4, 117*4, 13*4, 77*4, 45*4, 109*4, 29*4, 93*4, 61*4, 125*4
	.dc.w	3*4, 67*4, 35*4, 99*4, 19*4, 83*4, 51*4, 115*4, 11*4, 75*4, 43*4, 107*4, 27*4, 91*4, 59*4, 123*4
	.dc.w	7*4, 71*4, 39*4, 103*4, 23*4, 87*4, 55*4, 119*4, 15*4, 79*4, 47*4, 111*4, 31*4, 95*4, 63*4, 127*4
#endif
	.dc.w	0*4, 64*4, 32*4, 96*4, 16*4, 80*4, 48*4, 112*4, 8*4, 72*4, 40*4, 104*4, 24*4, 88*4, 56*4, 120*4
	.dc.w	4*4, 68*4, 36*4, 100*4, 20*4, 84*4, 52*4, 116*4, 12*4, 76*4, 44*4, 108*4, 28*4, 92*4, 60*4, 124*4
	.dc.w	2*4, 66*4, 34*4, 98*4, 18*4, 82*4, 50*4, 114*4, 10*4, 74*4, 42*4, 106*4, 26*4, 90*4, 58*4, 122*4
	.dc.w	6*4, 70*4, 38*4, 102*4, 22*4, 86*4, 54*4, 118*4, 14*4, 78*4, 46*4, 110*4, 30*4, 94*4, 62*4, 126*4
#endif



;----------------------------------------------------------------------------;
#ifndef INPUT_NOISE
.global fft_input_128
.func fft_input_128
fft_input_128:
	pushw	T2H,T2L
	pushw	AH,AL
	pushw	YH,YL

	movw	XL, EL				;X = array_src;
	movw	YL, DL				;Y = array_bfly;
	clr	EH				;Zero
	ldiw	ZH,ZL, tbl_window_128		;Z = &tbl_window[0];
	ldiw	AH,AL, FFT_N_128			;A = FFT_N;
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
.global fft_execute_128
.func fft_execute_128
fft_execute_128:
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
	ldiw	XH,XL, FFT_N_128/2			;X = FFT_N/2;
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
#if FFT_N_128 >= 128
	sbrs	T10H, FFT_B_128 - 7			;while(T10 < pi)
#else
	sbrs	T10L, FFT_B_128 + 1
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
.global fft_output_128
.func fft_output_128
fft_output_128:
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
	ldiw	AH,AL, FFT_N_128			;A = FFT_N; (plus/minus)
#else
	ldiw	AH,AL, FFT_N_128 / 2		;A = FFT_N / 2; (plus only)
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
.global fmuls_f_128
.func fmuls_f_128
fmuls_f_128:
	movw	CL, EL				;C = E;
	clr	EH	;Zero
	FMULS16	ZH,ZL,XH,XL, CH,CL, DH,DL	;Z:X = C * D;
	movw	EL, ZL
	clr	r1
	ret
.endfunc

