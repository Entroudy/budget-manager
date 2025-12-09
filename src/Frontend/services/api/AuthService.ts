import type { AxiosRequestConfig } from 'axios'
import { api } from '@/services/api/index'
import type { UserLogin } from '@/models/UserModel'

export default class AuthAPI {
  async login (userInfo: UserLogin) {
    const { data } = await api.post('authenticate/login', <AxiosRequestConfig['data']>{
      email: userInfo.email,
      password: userInfo.password,
      tokenExpiration: userInfo.rememberMe != null ? 2159 : 24
    })

    return data || undefined
  }
}
