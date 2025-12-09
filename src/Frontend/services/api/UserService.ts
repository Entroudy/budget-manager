import type { AxiosRequestConfig } from 'axios'
import { api } from '@/services/api/index'
import type { UserType } from '@/models/UserModel'
import type { ParamsType } from '@/models'

export default class UserAPI {
  async getUsers (params: ParamsType) {
    const { data } = (await api.query('users', { params }))
    return data || undefined
  }

  async createUser (user: UserType) {
    const { data } = (await api.post('users', <AxiosRequestConfig['data']>{
      lastName: user.lastName,
      firstName: user.firstName,
      nickName: user.nickName,
      email: user.email,
      password: user.password,
    }))

    return data || undefined
  }

  async getUser (id: string) {
    const { data } = (await api.get(`users/${id}`))
    return data || undefined
  }

  async deleteUser (id: string) {
    const { data } = (await api.delete(`users/${id}`))
    return data || undefined
  }

  async updateUser (userId: string, user: UserType) {
    const { data } = (await api.put(`users/${userId}`, <AxiosRequestConfig['data']>{
      lastName: user.lastName,
      firstName: user.firstName,
      nickName: user.nickName,
      email: user.email,
      password: user.password,
    }))

    return data || undefined
  }

  async changePassword (userId: string, password: string) {
    const { data } = (await api.patch(`users/${userId}/change-password`, <AxiosRequestConfig['data']>{
      password: password,
    }))

    return data || undefined
  }
}
