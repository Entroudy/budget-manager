import type { BasePagination, BaseResponseProps } from './'

export enum PaymentMethod {
  Cash = 0,
  Card = 1
}

export type Income = {
  id?: string
  applicationUserId?: string
  paymentMethod: PaymentMethod
  amount: number
  description?: string
  date: string
}

export type BaseUser = {
  lastName: string
  firstName: string
  nickName: string
  email: string
  password: string
  confirmPassword: string
}

export type UserLogin = {
  email: string
  password: string
  tokenExpiration: number
  rememberMe: boolean
}

export type UserRole = 'User' | 'Admin'

export type UserCreate = BaseUser & {
  role?: UserRole
}

export type UserUpdate ={
  id: string
  lastName: string
  firstName: string
  nickName: string
  email: string
  role?: UserRole
  incomes?: Income[]
}

export type ChangePassword ={
  id: string
  password: string
  confirmPassword: string
}

export type UserListItem = BaseUser & {
  id: string
  roles?: UserRole[]
  incomes?: Income[]
}

export type UserType = UserCreate
export type UserListTypes = UserListItem[]

/* API RESPONSE MODELS */
export type UserItemResponse = BaseResponseProps & UserUpdate & {
  roles?: UserRole[]
}

export type PaginatedUserListResponse<T> = BasePagination & {
  items: T[]
}
export type UserListResponse = PaginatedUserListResponse<UserListItem>
