import { getInitialLocale, normalizeLocale } from '~/utils/locale'

export default defineNuxtPlugin((nuxtApp) => {
  const i18n = nuxtApp.$i18n as { locale: { value: string } } | undefined
  if (!i18n) {
    return
  }

  i18n.locale.value = normalizeLocale(getInitialLocale())
})
