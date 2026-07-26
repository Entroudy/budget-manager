import enMessages from './locales/en.json'
import huMessages from './locales/hu.json'

export default defineI18nConfig(() => {
  return {
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages: {
      en: enMessages,
      hu: huMessages
    }
  }
})
