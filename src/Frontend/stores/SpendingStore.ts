import { defineStore } from 'pinia'
import type { SpendingCreate, SpendingItemResponse, SpendingUpdate } from '~/models/SpendingModel'
import type { ParamsType } from '@/models'

export const useSpendingStore = defineStore('spending', {
  state: () => {
    return {
      spendings: [] as SpendingItemResponse[],
    }
  },
  actions: {
    async createSpending (spending: SpendingCreate) {
      try {
        const data = await this.api.spending.createSpending(spending)
        return data
      } catch (error) {
        return null
      }
    },
    async getSpendings (params: ParamsType) {
      try {
        const data = await this.api.spending.getSpendings(params)
        if (data) {
          this.spendings = data.results
          return {
            currentPage: data.currentPage,
            totalPages: data.totalPages,
            totalCount: data.totalCount
          }
        }
      } catch (error) {
        return null
      }
    },
    async getSpending (id: string) {
      try {
        const data = await this.api.spending.getSpending(id)
        return data
      } catch (error) {
        return null
      }
    },
    async editSpending (spending: SpendingUpdate) {
      try {
        const data = await this.api.spending.editSpending(spending)
        return data
      } catch (error) {
        return null
      }
    },
    async deleteSpending (id: string) {
      try {
        const data = await this.api.spending.deleteSpending(id)
        return data
      } catch (error) {
        return null
      }
    },
  }
})
