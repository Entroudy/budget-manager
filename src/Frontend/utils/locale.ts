import { getUser as readStoredUser, saveUser } from '~/services/api/JwtService'

export type AppLocale = 'en' | 'hu'

export const LOCALE_STORAGE_KEY = 'budget-manager-locale'

export const normalizeLocale = (locale?: string | null): AppLocale => {
  return locale === 'hu' ? 'hu' : 'en'
}

export const getStoredLocale = (): AppLocale | null => {
  if (!process.client) {
    return null
  }

  const locale = window.localStorage.getItem(LOCALE_STORAGE_KEY)
  return locale ? normalizeLocale(locale) : null
}

export const getStoredUserLocale = (): AppLocale | null => {
  if (!process.client) {
    return null
  }

  const rawUser = readStoredUser()
  if (!rawUser) {
    return null
  }

  try {
    const user = JSON.parse(rawUser)
    return user?.locale ? normalizeLocale(user.locale) : null
  } catch {
    return null
  }
}

export const getInitialLocale = (): AppLocale => {
  return getStoredUserLocale() ?? getStoredLocale() ?? 'en'
}

export const persistLocalePreference = (locale: AppLocale): void => {
  if (!process.client) {
    return
  }

  window.localStorage.setItem(LOCALE_STORAGE_KEY, locale)
}

export const persistLocaleToStoredUser = (locale: AppLocale): void => {
  if (!process.client) {
    return
  }

  const rawUser = readStoredUser()
  if (!rawUser) {
    return
  }

  try {
    const user = JSON.parse(rawUser)
    saveUser(JSON.stringify({ ...user, locale }))
  } catch {
    // Ignore malformed storage values.
  }
}
