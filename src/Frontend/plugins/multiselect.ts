
import Multiselect from 'vue-multiselect'
import 'vue-multiselect/dist/vue-multiselect.css';

export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.component("multiselect", Multiselect);
});
