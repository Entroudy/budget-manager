import type { BasePagination, BaseResponseProps } from './index'

export enum PaymentMethod {
  Cash = 0,
  Card = 1
}

export type SpendingItemResponse = BaseResponseProps & {
  id: number
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
  item: string
  amount: number
  paymentMethod: PaymentMethod
  comment?: string | null
  color?: string | null
  date: string
}

export type SpendingUpdate = {
  id: number
  item: string
  amount: number
  paymentMethod: PaymentMethod
  comment?: string | null
  color?: string | null
  date: string
}
