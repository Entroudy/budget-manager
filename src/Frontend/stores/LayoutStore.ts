import { defineStore } from 'pinia'

export const useLayoutStore = defineStore('layout', {
  state: () => {
    return {
      isOverlayVisible: false,
    }
  },
  actions: {
    setOverlay (value: boolean) {
      this.isOverlayVisible = value
      if (value) {
        document.body.classList.add('overflow-hidden');
      } else {
        document.body.classList.remove('overflow-hidden');
      }
    },
  }
})
