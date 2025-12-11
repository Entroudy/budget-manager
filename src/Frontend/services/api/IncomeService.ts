import type { AxiosRequestConfig } from 'axios'
import { api } from '@/services/api/index'
import type { Income } from '@/models/UserModel'

export default class IncomeAPI {
  async getIncomesByUserId (userId: string) {
    const { data } = (await api.get(`incomes/user/${userId}`))
    return data || undefined
  }

  async createIncome (income: Income, userId: string) {
    const { data } = (await api.post('incomes', <AxiosRequestConfig['data']>{
      applicationUserId: userId,
      amount: income.amount,
      paymentMethod: income.paymentMethod,
      comment: income.description || null,
    }))

    return data || undefined
  }

  async updateIncome (incomeId: string, income: Income, userId: string) {
    const { data } = (await api.put(`incomes/${incomeId}`, <AxiosRequestConfig['data']>{
      applicationUserId: userId,
      amount: income.amount,
      paymentMethod: income.paymentMethod,
      comment: income.description || null,
    }))

    return data || undefined
  }

  async deleteIncome (incomeId: string) {
    const { data} = (await api.delete(`incomes/${incomeId}`))
    return data || undefined
  }
}
