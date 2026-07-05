import { defineStore } from 'pinia'
import type { Income } from '~/models/UserModel'

export const useIncomeStore = defineStore('income', {
  state: () => {
    return {
      incomes: [] as Income[],
    }
  },
  actions: {
    async getIncomesByUserId (userId: string) {
      try {
        const data = await this.api.income.getIncomesByUserId(userId)
        if (data) {
          this.incomes = [
            ...this.incomes.filter(income => income.applicationUserId !== userId),
            ...data
          ]
          return data
        }
      } catch (error) {
        console.error('Error fetching incomes:', error)
        return null
      }
    },
    async createIncome (income: Income, userId: string) {
      try {
        const data = await this.api.income.createIncome(income, userId)
        return data
      } catch (error) {
        return null
      }
    },
    async updateIncome (incomeId: string, income: Income, userId: string) {
      try {
        const data = await this.api.income.updateIncome(incomeId, income, userId)
        return data
      } catch (error) {
        return null
      }
    },
    async deleteIncome (incomeId: string) {
      try {
        const data = await this.api.income.deleteIncome(incomeId)
        return data
      } catch (error) {
        return null
      }
    },
    setIncomes (data: Income[]) {
      this.incomes = data
    }
  }
})
