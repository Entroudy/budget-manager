import { defineStore } from 'pinia'
import { saveUser, getUser, removeUser } from '~/services/api/JwtService'
import type { UserLogin } from '@/models/UserModel'

export const useAuthStore = defineStore('auth', {
  state: () => {
    return {
      account: {
        email: '',
        token: ''
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
        data.email = userInfo.email
        if (data) {
          saveUser(JSON.stringify(data))
          this.account.token = data.token
          navigateTo('/')
        }
      } catch (error) {
        return null
      }
    },
    logout () {
      removeUser()
      this.account.token = ''

      navigateTo('/login')
    },
    getUser () {
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          try {
            const user = JSON.parse(getUser() as any)
            if (user !== null) {
              this.account = user
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
