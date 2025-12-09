
import Datepicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import '../assets/css/datepicker/datepicker.css';

export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.component("Datepicker", Datepicker);
});
