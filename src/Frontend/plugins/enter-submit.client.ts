import { defineNuxtPlugin } from '#app';
import type { DirectiveBinding } from 'vue';

export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.directive('enter-submit', {
    mounted(el: HTMLElement, binding: DirectiveBinding) {
      el.addEventListener('keydown', (e: KeyboardEvent) => {
        if (e.key === 'Enter') {
          if (e.target && e.target.tagName !=='DIV') {
            e.preventDefault();
            if (binding.value && typeof binding.value === 'function') {
              binding.value();
            }
          }
        }
      });
    },
    unmounted(el: HTMLElement) {
      el.removeEventListener('keydown', () => {});
    }
  });
});
