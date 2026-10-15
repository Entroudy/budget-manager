import { defineStore } from 'pinia'
import type { UserCreate, UserListTypes, UserRole, UserUpdate } from '~/models/UserModel'
import type { ParamsType } from '@/models'

export const useUserStore = defineStore('user', {
  state: () => {
    return {
      users: [] as UserListTypes,
      roles: [] as UserRole[],
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
    async getRoles () {
      try {
        const data = await this.api.user.getRoles()
        if (Array.isArray(data)) {
          this.roles = data
          return data
        }
        return []
      } catch (error) {
        console.error('Error fetching roles:', error)
        return []
      }
    },
    async createUser (userType: UserCreate) {
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
    async updateUser (applicationUserId: string, userType: UserUpdate) {
      try {
         const data = await this.api.user.updateUser(applicationUserId, userType)
        return data
      } catch (error) {
        console.error('Error updating user:', error)
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
    },
  }
})
