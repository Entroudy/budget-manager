import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useGlobalStore = defineStore('global', () => {
  const activePageTitleKey = ref('');

  function setActivePageTitle(value: string) {
    activePageTitleKey.value = value;
  }

  return { activePageTitleKey, setActivePageTitle };
});
