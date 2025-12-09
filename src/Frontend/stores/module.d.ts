import "pinia"
import { APIService } from "~/services/api"

declare module "pinia" {

  export interface PiniaCustomProperties {
    api: APIService
  }
}
