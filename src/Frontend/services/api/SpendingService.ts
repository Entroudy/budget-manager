import type { AxiosRequestConfig } from 'axios'
import { api } from '@/services/api/index'
import type { SpendingItemResponse, SpendingCreate, SpendingListResponse, SpendingUpdate } from '@/models/SpendingModel'
import type { ParamsType } from '@/models'

export default class SpendingAPI {
  async createSpending (spending: SpendingCreate): Promise<string | undefined> {
    const { data } = (await api.post('/spendings', <AxiosRequestConfig['data']>{
      applicationUserId: spending.applicationUserId,
      item: spending.item,
      amount: spending.amount,
      paymentMethod: spending.paymentMethod,
      comment: spending.comment,
      color: spending.color,
      date: spending.date
    }))
    return data || undefined
  }

  async getSpendings (params: ParamsType): Promise<SpendingListResponse | undefined> {
    const { data } = (await api.query('/spendings', { params }))
    return data || undefined
  }

  async getSpending (id: string): Promise<SpendingItemResponse | undefined> {
    const { data } = (await api.get(`/spendings/${id}`))
    return data || undefined
  }

  async editSpending (spending: SpendingUpdate): Promise<{} | undefined> {
    const { data } = (await api.put(`/spendings/${spending.id}`, <AxiosRequestConfig['data']>{
      applicationUserId: spending.applicationUserId,
      item: spending.item,
      amount: spending.amount,
      paymentMethod: spending.paymentMethod,
      comment: spending.comment,
      color: spending.color,
      date: spending.date
    }))
    return data || undefined
  }

  async deleteSpending (id: string): Promise<{} | undefined> {
    const { data } = (await api.delete(`/spendings/${id}`))
    return data || undefined
  }
}
