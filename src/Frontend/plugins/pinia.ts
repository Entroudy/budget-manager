import { createPinia } from 'pinia'
import { api } from '@/services/api/index'

export default defineNuxtPlugin((nuxtApp) => {
  const pinia = createPinia()

  pinia.use(({ store }) => {
    store.api = markRaw(api)
  })

  nuxtApp.vueApp.use(pinia)
})
