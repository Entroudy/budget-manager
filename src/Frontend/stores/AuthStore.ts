import { defineStore } from 'pinia'
import { saveUser, getUser, removeUser } from '~/services/api/JwtService'
import type { UserLogin } from '@/models/UserModel'
import { getInitialLocale, normalizeLocale, persistLocalePreference, persistLocaleToStoredUser, type AppLocale } from '~/utils/locale'

export const useAuthStore = defineStore('auth', {
  state: () => {
    return {
      account: {
        email: '',
        token: '',
        locale: getInitialLocale() as AppLocale
      }
    }
  },
  getters: {
    isLoggedIn (): boolean {
      return this.account.token !== ''
    }
  },
  actions: {
    async login (userInfo: UserLogin) {
      try {
        const data = await this.api.auth.login(userInfo)
        if (data) {
          data.email = userInfo.email
          const locale = normalizeLocale(getInitialLocale())
          this.account = {
            email: userInfo.email,
            token: data.token,
            locale
          }
          persistLocalePreference(locale)
          saveUser(JSON.stringify(this.account))
          navigateTo('/')
        }
      } catch (error) {
        return null
      }
    },
    logout () {
      removeUser()
      this.account = {
        email: '',
        token: '',
        locale: getInitialLocale()
      }

      navigateTo('/login')
    },
    setLocale (locale: AppLocale) {
      const normalized = normalizeLocale(locale)
      this.account.locale = normalized
      persistLocalePreference(normalized)

      if (this.account.token) {
        saveUser(JSON.stringify(this.account))
      } else {
        persistLocaleToStoredUser(normalized)
      }
    },
    getUser () {
      try {
        const storedUser = getUser()
        if (!storedUser) {
          return Promise.reject(new Error("Item with key 'user' not found in localStorage."))
        }

        const user = JSON.parse(storedUser)
        this.account = {
          email: user.email ?? '',
          token: user.token ?? '',
          locale: normalizeLocale(user.locale ?? getInitialLocale())
        }
        persistLocalePreference(this.account.locale)
        return Promise.resolve(user)
      } catch (error: any) {
        return Promise.reject(error)
      }
    }
  }
})
