import { alpha, digits, required, max } from 'vee-validate/dist/rules';
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from 'vee-validate';

setInteractionMode('eager');

extend('alpha', {
    ...alpha,
    message: '{_field_} can not contain special character except spaces'
});
extend('digits', {
    ...digits
});
extend('required', {
    ...required,
    message: '{_field_} can not be empty'
});
extend('max', {
    ...max,
    message: '{_field_} may not be greater than {length} characters'
});
extend('positive', {
    validate: value => value && !isNaN(value) && value > 0
});
extend('alpha-numeric-spaces', {
    message: '{_field_} can only contain alphabets numbers and spaces',
    validate: value => value && /^[a-z\d\-_\s]+$/i.test(value)
});

export default {
    components: {
        ValidationProvider,
        ValidationObserver
    }
};
