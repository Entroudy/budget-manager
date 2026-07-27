export type ParamsType = {
  id?: string
  only_closed?: boolean
  page_number?: number
  page_size?: number
  search_value?: string
  sort_By?: string
  ascending?: boolean
  without_state?: boolean
  without_sprint?: boolean
  just_actives?: boolean
  just_delayed?: boolean
  is_archived?: boolean
  expire_by_date?: string
  activation_state?: boolean
  tray_type?: string
  delivery_zone?: string
  from_date?: string
  to_date?: string
  garden_id?: string[] | string
  garden_ids?: string[] | string
  partner_id?: string[]
  statuses?: string[]
  customer_partner_id?: string
  supplier_partner_id?: string
  product_category?: string
  product_categories?: string[]
  exclude_null_inventory?: boolean
  exclude_closed_provisions?: boolean
}


export type BasePagination = {
  currentPage: number
  totalPages: number
  pageSize: number
  totalCount: number
}


export type BaseSelectItem = {
  value: string
  name: string
}

export type PaginatedSelectItemListResponse<T> = BasePagination & {
  items: T[]
}

export type SelectItemResponse = PaginatedSelectItemListResponse<BaseSelectItem>

export type BaseSelectItemList = BaseSelectItem[]


export type BaseResponseProps = {
  createdWhen: string
  createdBy: string
  modifiedWhen: string | null
  modifiedBy: string | null
}
