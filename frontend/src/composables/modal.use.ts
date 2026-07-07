export type ModalStyle = 'centered' | 'filled' | 'fullscreen' | 'side-right';

export type OnShowDetails<T> = {
    style?: ModalStyle;
    component: any | null;
    componentProps: T;
    onClose?: () => void;
};

type OnShowEvent<T> = (details: OnShowDetails<T>) => void;
type OnHideEvent = () => void;

const events: {
    show: OnShowEvent<any>,
    hide: OnHideEvent,
} = {
    show: () => {},
    hide: () => {},
};

export const useModal = function () {
    return {
        show<T>(details: OnShowDetails<T>): void {
            events.show(details);
            document.body.classList.add('wjb-modal-visible');
        },

        hide(): void {
            events.hide();
            document.body.classList.remove('wjb-modal-visible');
        },

        setOnShow(event: OnShowEvent<any>): void {
            events.show = event;
        },

        setOnHide(event: OnHideEvent): void {
            events.hide = event;
        },
    };
};