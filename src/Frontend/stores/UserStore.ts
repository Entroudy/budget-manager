import { defineStore } from 'pinia'
import type { UserListTypes, UserType } from '~/models/UserModel'
import type { ParamsType } from '@/models'

export const useUserStore = defineStore('user', {
  state: () => {
    return {
      users: [] as UserListTypes,
    }
  },
  actions: {
    async getUsers (params: ParamsType) {
      try {
       const data = await this.api.user.getUsers(params)
       if (data) {
        // Handle both array and paginated response formats
        if (Array.isArray(data)) {
          this.users = data
          return {
            currentPage: 1,
            pageSize: data.length,
            totalCount: data.length,
            totalPages: 1
          }
        } else if (data.results) {
          // Backend returns results array
          this.users = data.results
          return {
            currentPage: data.currentPage || 1,
            pageSize: params.page_size || 10,
            totalCount: data.totalCount || 0,
            totalPages: data.totalPages || data.totalPage || Math.ceil((data.totalCount || 0) / (params.page_size || 10))
          }
        } else if (data.items) {
          this.users = data.items
          return {
            currentPage: data.currentPage,
            pageSize: data.pageSize,
            totalCount: data.totalCount,
            totalPages: data.totalPages
          }
        }
      }
      } catch (error) {
        console.error('Error fetching users:', error)
        return null
      }
    },
    async createUser (userType: UserType) {
      try {
       const data = await this.api.user.createUser(userType)
        return data
      } catch (error) {
        return null
      }
    },
    async deleteUser (id: string) {
      try {
         const data = await this.api.user.deleteUser(id)
        return data
      } catch (error) {
        return null
      }
    },
    async updateUser (applicationUserId: string, userType: UserType) {
      try {
         const data = await this.api.user.updateUser(applicationUserId, userType)
        return data
      } catch (error) {
        return null
      }
    },
    async changePassword (applicationUserId: string, password: string) {
      try {
         const data = await this.api.user.changePassword(applicationUserId, password)
        return data
      } catch (error) {
        return null
      }
    },
    setUsers (data: UserListTypes) {
      this.users = data
    }
  }
})
