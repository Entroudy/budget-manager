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
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          try {
            const user = JSON.parse(getUser() as any)
            if (user !== null) {
              this.account = {
                email: user.email ?? '',
                token: user.token ?? '',
                locale: normalizeLocale(user.locale ?? getInitialLocale())
              }
              persistLocalePreference(this.account.locale)
              resolve(user)
            } else {
              reject('Item with key \'user\' not found in localStorage.')
            }
          } catch (error: any) {
            reject(error.message)
          }
        }, 50)
      })
    }
  }
})
