import type { AxiosRequestConfig } from 'axios'
import { api } from '@/services/api/index'
import type { UserCreate, UserUpdate } from '@/models/UserModel'
import type { ParamsType } from '@/models'

export default class UserAPI {
  async getUsers (params: ParamsType) {
    const { data } = (await api.query('applicationUsers', { params }))
    return data || undefined
  }

  async getRoles () {
    const { data } = (await api.get('applicationUsers/roles'))
    return data || undefined
  }

  async createUser (user: UserCreate) {
    const { data } = (await api.post('applicationUsers', <AxiosRequestConfig['data']>{
      lastName: user.lastName,
      firstName: user.firstName,
      nickName: user.nickName,
      email: user.email,
      password: user.password,
      role: user.role,
    }))

    return data || undefined
  }

  async getUser (id: string) {
    const { data } = (await api.get(`applicationUsers/${id}`))
    return data || undefined
  }

  async deleteUser (id: string) {
    const { data } = (await api.delete(`applicationUsers/${id}`))
    return data || undefined
  }

  async updateUser (applicationUserId: string, user: UserUpdate) {
    const { data } = (await api.put(`applicationUsers/${applicationUserId}`, <AxiosRequestConfig['data']>{
      lastName: user.lastName,
      firstName: user.firstName,
      nickName: user.nickName,
      email: user.email,
      role: user.role,
    }))

    return data || undefined
  }

  async changePassword (applicationUserId: string, password: string) {
    const { data } = (await api.patch(`applicationUsers/${applicationUserId}/change-password`, <AxiosRequestConfig['data']>{
      password: password,
    }))

    return data || undefined
  }
}
