import { PaymentMethod } from '~/models/SpendingModel'

export type SpendingViewMode = 'list' | 'grid'

export type SystemPreferences = {
  defaultPaymentMethod: PaymentMethod
  spendingViewMode: SpendingViewMode
  spendingPageSize: number
}

export const SYSTEM_PREFERENCES_KEY = 'budget-manager-system-preferences'

export const defaultSystemPreferences: SystemPreferences = {
  defaultPaymentMethod: PaymentMethod.Card,
  spendingViewMode: 'list',
  spendingPageSize: 50,
}

export const loadSystemPreferences = (): SystemPreferences => {
  if (typeof localStorage === 'undefined') return { ...defaultSystemPreferences }
  try {
    const saved = JSON.parse(localStorage.getItem(SYSTEM_PREFERENCES_KEY) || '{}')
    return {
      defaultPaymentMethod: saved.defaultPaymentMethod === PaymentMethod.Cash ? PaymentMethod.Cash : PaymentMethod.Card,
      spendingViewMode: saved.spendingViewMode === 'grid' ? 'grid' : 'list',
      spendingPageSize: [10, 25, 50, 100].includes(Number(saved.spendingPageSize)) ? Number(saved.spendingPageSize) : 50,
    }
  } catch {
    return { ...defaultSystemPreferences }
  }
}

export const saveSystemPreferences = (preferences: SystemPreferences): void => {
  if (typeof localStorage === 'undefined') return
  localStorage.setItem(SYSTEM_PREFERENCES_KEY, JSON.stringify(preferences))
}
