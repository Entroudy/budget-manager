import axios, { type AxiosRequestConfig, type AxiosResponse } from 'axios'
import { useNuxtApp, type NuxtApp } from '#app'
import { storeToRefs } from 'pinia'
import { useAuthStore } from '@/stores/AuthStore'
import AuthAPI from '@/services/api/AuthService'
import UserAPI from '@/services/api/UserService'
import SpendingAPI from '~/services/api/SpendingService'

class APIService {
  auth: AuthAPI
  user: UserAPI
  spending: SpendingAPI
  constructor() {
    this.auth = new AuthAPI()
    this.user = new UserAPI()
    this.spending = new SpendingAPI()
  }

  public axiosInstance() {
    const { $event }: NuxtApp = useNuxtApp()
    const { account } = storeToRefs(useAuthStore())
    const { logout } = useAuthStore()

    const defaultUrl = useRuntimeConfig().public.apiUrl
    const api = axios.create({
      baseURL: defaultUrl,
    })


    // Add a request interceptor
    api.interceptors.request.use(
      function (config) {
        // Do something before request is sent
        config.headers.Authorization = `Bearer ${account.value?.token}`
        return config
      },
      function (error) {
        // Do something with request error
        return Promise.reject(error)
      }
    )

    // Add a response interceptor
    api.interceptors.response.use(
      function (response) {
        // Any status code that lie within the range of 2xx cause this function to trigger
        // Do something with response data
        return response
      },
      function (error) {
        // Any status codes that falls outside the range of 2xx cause this function to trigger
        // Do something with response error
        if (error.response.status === 401) {
          return logout()
        }
        $event('notification', { type: 'error', message: error.response.data.title })
        console.log(error)
        return Promise.reject(error)
      }
    )

    return api
  }

  /**
   * @description send the GET HTTP request
   * @param resource: string
   * @param params: AxiosRequestConfig
   * @returns Promise<AxiosResponse>
   */
  query(resource: string, params: AxiosRequestConfig): Promise<AxiosResponse> {
    return this.axiosInstance().get(resource, params)
  }

  /**
   * @description send the GET HTTP request
   * @param resource: string
   * @param slug: string
   * @returns Promise<AxiosResponse>
   */
  get(resource: string, slug = '' as string): Promise<AxiosResponse> {
    return this.axiosInstance().get(`${resource}/${slug}`)
  }

  /**
   * @description send the GET HTTP request
   * @param resource: string
   * @param params: AxiosRequestConfig
   * @returns Promise<AxiosResponse>
   */
  getAll(resource: string, params: AxiosRequestConfig = <AxiosRequestConfig>{}): Promise<AxiosResponse> {
    if (params.params) {
      params.params.pageSize = 999999999
    } else {
      params.params = { pageSize: 999999999 }
    }
    return this.query(resource, params)
  }

  /**
   * @description set the POST HTTP request
   * @param resource: string
   * @param params: AxiosRequestConfig
   * @param customHeaders [key: string]: string
   * @returns Promise<AxiosResponse>
   */
  post(
    resource: string,
    params: AxiosRequestConfig,
    customHeaders?: { [key: string]: string },
  ): Promise<AxiosResponse> {
    const axiosConfig: AxiosRequestConfig = {
      headers: customHeaders || {},
      ...params,
    }

    return this.axiosInstance().post(`${resource}`, params.data || params, axiosConfig) // Make sure to send the correct data
  }

  /**
   * @description Send the PUT HTTP request
   * @param resource: string
   * @param params: AxiosRequestConfig
   * @returns Promise<AxiosResponse>
   */
  put(resource: string, params: AxiosRequestConfig): Promise<AxiosResponse> {
    return this.axiosInstance().put(`${resource}`, params)
  }

  /**
   * @description Send the PATCH HTTP request
   * @param resource: string
   * @param params: AxiosRequestConfig
   * @returns Promise<AxiosResponse>
   */
  patch(resource: string, params?: AxiosRequestConfig): Promise<AxiosResponse> {
    return this.axiosInstance().patch(`${resource}`, params)
  }

  /**
   * @description Send the DELETE HTTP request
   * @param resource: string
   * @returns Promise<AxiosResponse>
   */
  delete(resource: string, params?: AxiosRequestConfig): Promise<AxiosResponse> {
    return this.axiosInstance().delete(resource, params)
  }

  deleteAll(resource: string, data?: any): Promise<AxiosResponse> {
    return this.axiosInstance().patch(resource, data)
  }

  export(resource: string, data?: any): Promise<AxiosResponse> {
    return this.axiosInstance().post(`${resource}`, data)
  }

  import(resource: string, formFile: FormData): Promise<AxiosResponse> {
    return this.axiosInstance().post(`${resource}`, formFile)
  }

  uploadImage(resource: string, image: FormData): Promise<AxiosResponse> {
    return this.axiosInstance().post(`${resource}`, image)
  }

  removeImage(resource: string): Promise<AxiosResponse> {
    return this.axiosInstance().delete(`${resource}`)
  }
}

const api = new APIService()

export { api, APIService }
