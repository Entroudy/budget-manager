import type { BasePagination, BaseResponseProps } from './index'

export enum PaymentMethod {
  Cash = 0,
  Card = 1
}

export type SpendingItemResponse = BaseResponseProps & {
  id: string
  applicationUserId: string
  item: string
  amount: number
  paymentMethod: PaymentMethod
  comment: string | null
  color: string | null
  date: string
}

export type SpendingListResponse = BasePagination & {
  results: SpendingItemResponse[]
}

export type SpendingCreate = {
  applicationUserId: string
  item: string
  amount: number
  paymentMethod: PaymentMethod
  comment?: string | null
  color?: string | null
  date: string
}

export type SpendingUpdate = {
  id: string
  applicationUserId: string
  item: string
  amount: number
  paymentMethod: PaymentMethod
  comment?: string | null
  color?: string | null
  date: string
}
