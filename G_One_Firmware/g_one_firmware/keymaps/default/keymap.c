#include QMK_KEYBOARD_H
#include "print.h"

enum layer_names {
    _BASE,
    _FN
};

enum custom_keycodes {
    MC_TOP = SAFE_RANGE,
	MC_MID,
	MC_ADC,
    MC_SUP,
	MC_JG,
    IOT_LED,
    IOT_MULTI,
};

const uint16_t PROGMEM keymaps[][MATRIX_ROWS][MATRIX_COLS] = {
    /*
    { KC_NO, K01,   K02,   K03,   K04,   K05,   KC_NO, K07 }, \
	{ K10,   K11,   K12,   K13,   K14,   K15,   K16,   K17 }, \
	{ K20,   K21,   K22,   K23,   K24,   K25,   K26,   K27 }, \
	{ K30,   K31,   K32,   K33,   K34,   K35,   K36,   KC_NO }, \
	{ K40,   K41,   K42,   K43,   K44,   K45,   KC_NO, KC_NO }, \
	{ K50,   K51,   K52,   K53,   K54,   KC_NO, KC_NO, KC_NO }  \
    */

    /* Base */
    [_BASE] = LAYOUT(
                    KC_ESC,     KC_F1,      KC_F2,      KC_F3,      KC_F4,      KC_F5,
        MC_TOP,     KC_GRV,     KC_1,       KC_2,       KC_3,       KC_4,       KC_5,       KC_6,
        MC_JG,      KC_TAB,     KC_Q,       KC_W,       KC_E,       KC_R,       KC_T,       KC_P,
        MC_MID,                 KC_A,       KC_S,       KC_D,       KC_F,                   KC_H,       IOT_LED,
        MC_ADC,     KC_LSFT,    KC_Z,                   KC_C,                   KC_B,                   IOT_MULTI,
        MC_SUP,     KC_LCTL,            KC_LALT,        KC_SPC,                                         MO(_FN)
    ),

    [_FN] = LAYOUT(
                    RESET,     KC_NO,       KC_NO,      KC_NO,      KC_NO,      KC_NO,
        KC_NO,      KC_NO,     KC_NO,       KC_NO,      KC_NO,      KC_NO,      KC_NO,      KC_NO,
        KC_NO,      KC_NO,     KC_NO,       KC_NO,      KC_NO,      KC_NO,      KC_NO,      KC_NO,
        KC_NO,                 KC_NO,       KC_NO,      KC_NO,      KC_NO,                  KC_NO,      NK_ON,
        KC_NO,      KC_NO,     KC_NO,                   KC_NO,                  KC_NO,                  NK_OFF,
        KC_NO,      KC_NO,                  KC_NO,      KC_NO,                                          KC_NO
    )
};

bool process_record_user(uint16_t keycode, keyrecord_t *record) {
    /*
    #ifdef CONSOLE_ENABLE
        uprintf("KL: kc: 0x%04X, col: %u, row: %u, pressed: %b, time: %u\n", keycode, record->event.key.col, record->event.key.row, record->event.pressed, record->event.time);
    #endif
    */

    switch (keycode) {
    case MC_TOP:
        if (record->event.pressed) {
            SEND_STRING("x"SS_TAP(X_ENT));
        } else {

        }
        break;

	case MC_MID:
        if (record->event.pressed) {
            SEND_STRING("ae"SS_TAP(X_ENT));
        } else {

        }
        break;

	case MC_ADC:
        if (record->event.pressed) {
            SEND_STRING("de"SS_TAP(X_ENT));
        } else {

        }
        break;

	case MC_SUP:
        if (record->event.pressed) {
            SEND_STRING("tv"SS_TAP(X_ENT));
        } else {

        }
        break;

    case MC_JG:
        if (record->event.pressed) {
            SEND_STRING("wr"SS_TAP(X_ENT));
        } else {

        }
        break;

    case IOT_LED:
        if (record->event.pressed) {
            #ifdef CONSOLE_ENABLE
                print("LED");
            #endif
        } else {

        }
        break;

    case IOT_MULTI:
        if (record->event.pressed) {
            #ifdef CONSOLE_ENABLE
                print("Power_Strip");
            #endif
        }
        break;
    }

    return true;
};
