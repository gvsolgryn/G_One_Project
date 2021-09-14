/* Copyright 2021 GVSolgryn
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#include QMK_KEYBOARD_H

// Defines names for use in layer keycodes and the keymap
enum layer_names {
    _BASE,
    _FN
};

// Defines the keycodes used by our macros in process_record_user
enum custom_keycodes {
    M_TOP,
    M_MID,
    M_ADC,
    M_SUP,
    M_JGC,
    IOT_1,
    IOT_2,
    IOT_3
};

// 키 설정 그림으로 나타내기
/*
 * ,-------------------------------------------.
 * |▓▓▓|▓▓▓|ESC|▓▓▓|F1 |F2 |F3 |F4 |▓▓▓|F5 |▓▓▓|
 * |-------------------------------------------|
 * |▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|▓▓▓|
 * |-------------------------------------------|
 * |M1 |▓▓▓| ` | 1 | 2 | 3 | 4 | 5 | 6 |▓▓▓|▓▓▓|
 * |-------------------------------------------|
 * |M2 |▓▓▓|TAP| Q | W | E | R | T | P |▓▓▓|▓▓▓|
 * |-------------------------------------------|
 * |M3 |▓▓▓|CAP| A | S | D | F | G | H |▓▓▓|IOT|
 * |-------------------------------------------|
 * |M4 |▓▓▓|SFT| Z | X | C | V | B |▓▓▓|▓▓▓|IOT|
 * |-------------------------------------------|
 * |M5 |▓▓▓|CTL|FN |ALT|SPC|▓▓▓|▓▓▓|▓▓▓|▓▓▓|_FN|
 * `-------------------------------------------'
 */
const uint16_t PROGMEM keymaps[][MATRIX_ROWS][MATRIX_COLS] = {
    /* Base */
    [_BASE] = LAYOUT(
               KC_ESC,  KC_F1,  KC_F2,   KC_F3,  KC_F4,          KC_F5,
        M_TOP, KC_GRV,  KC_1,   KC_2,    KC_3,   KC_4,   KC_5,   KC_6,
        M_JGC, KC_TAB,  KC_Q,   KC_W,    KC_E,   KC_R,   KC_T,   KC_P,
        M_MID, KC_CAPS, KC_A,   KC_S,    KC_D,   KC_F,   KC_G,   KC_H,    IOT_1,
        M_ADC, KC_LSFT, KC_Z,   KC_X,    KC_C,   KC_V,   KC_B,            IOT_2,
        M_SUP, KC_LCTL, KC_APP, KC_LOPT, KC_SPC,                          MO(_FN)
    ),
    [_FN] = LAYOUT(
                RESET, KC_NO, KC_NO, KC_NO, KC_NO,        KC_NO,
        KC_NO,  KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO,
        KC_NO,  KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO,
        KC_NO,  KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO,        KC_NO,
        KC_NO,  KC_NO, KC_NO, KC_NO, KC_NO, KC_NO, KC_NO,               KC_NO,
        KC_NO,  KC_NO, KC_NO, KC_NO, KC_NO,                             KC_NO
    )
};

bool process_record_user(uint16_t keycode, keyrecord_t *record) {
    switch (keycode) {
        case M_TOP:
            if (record->event.pressed) {
                // when keycode QMKBEST is pressed
                SEND_STRING("x"SS_TAP(X_ENT));
            } else {
                // when keycode QMKBEST is released
            }
            break;

        case M_JGC:
            if (record->event.pressed) {
                // when keycode QMKBEST is pressed
                SEND_STRING("wr"SS_TAP(X_ENT));
            } else {
                // when keycode QMKBEST is released
            }
            break;

        case M_MID:
            if (record->event.pressed) {
                // when keycode QMKBEST is pressed
                SEND_STRING("ae"SS_TAP(X_ENT));
            } else {
                // when keycode QMKBEST is released
            }
            break;

        case M_ADC:
            if (record->event.pressed) {
                // when keycode QMKBEST is pressed
                SEND_STRING("de"SS_TAP(X_ENT));
            } else {
                // when keycode QMKBEST is released
            }
            break;

        case M_SUP:
            if (record->event.pressed) {
                // when keycode QMKBEST is pressed
                SEND_STRING("tv"SS_TAP(X_ENT));
            } else {
                // when keycode QMKBEST is released
            }
            break;
    }
    return true;
}
