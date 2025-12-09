import type { BasePagination, BaseResponseProps } from './'

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

export type UserCreate = BaseUser

export type UserUpdate ={
  id: string
  lastName: string
  firstName: string
  nickName: string
  email: string
}

export type ChangePassword ={
  id: string
  password: string
  confirmPassword: string
}

export type UserListItem = BaseUser & {
  id: string
}

export type UserType = UserCreate
export type UserListTypes = UserListItem[]

/* API RESPONSE MODELS */
export type UserItemResponse = BaseResponseProps & UserUpdate

export type PaginatedUserListResponse<T> = BasePagination & {
  items: T[]
}
export type UserListResponse = PaginatedUserListResponse<UserListItem>
