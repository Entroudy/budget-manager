import enMessages from './locales/en.json'
import huMessages from './locales/hu.json'

const capitalizeFirstLetter = (value) => {
  if (typeof value !== 'string' || value.length === 0) {
    return value
  }

  return value.charAt(0).toLocaleUpperCase('hu-HU') + value.slice(1)
}

const mapTranslationValues = (value) => {
  if (typeof value === 'string') {
    return capitalizeFirstLetter(value)
  }

  if (Array.isArray(value)) {
    return value.map(mapTranslationValues)
  }

  if (value && typeof value === 'object') {
    return Object.fromEntries(
      Object.entries(value).map(([key, nestedValue]) => [key, mapTranslationValues(nestedValue)])
    )
  }

  return value
}

export default defineI18nConfig(() => {
  return {
    legacy: false,
    locale: 'en',
    fallbackLocale: 'en',
    messages: {
      en: enMessages,
      hu: mapTranslationValues(huMessages)
    }
  }
})
