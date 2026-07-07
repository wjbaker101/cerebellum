import { readonly, ref } from 'vue';

export const popupStylesAsList = [
    'error', 'success', 'warning', 'info',
] as const;
export type PopupStyle = typeof popupStylesAsList[number];

export interface IPopupTiggerDetails {
    style: PopupStyle;
    message: string;
}

const _details = ref<IPopupTiggerDetails>({
    style: 'info',
    message: '',
});

const _isVisible = ref<boolean>(false);

let timeout: number;

export const usePopup = function () {
    return {
        details: readonly(_details),
        isVisible: readonly(_isVisible),

        trigger(details: IPopupTiggerDetails): void {
            _details.value.style = details.style;
            _details.value.message = details.message;
            _isVisible.value = true;

            clearTimeout(timeout);
            timeout = setTimeout(() => {
                _isVisible.value = false;
            }, 4000);
        },

        hide() {
            clearTimeout(timeout);
            _isVisible.value = false;
        },
    };
};