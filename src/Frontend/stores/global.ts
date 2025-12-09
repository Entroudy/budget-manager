import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useGlobalStore = defineStore('global', () => {
  const activePageTitle = ref('');

  function setActivePageTitle(value: string) {
    activePageTitle.value = value;
  }

  return { activePageTitle, setActivePageTitle };
});
