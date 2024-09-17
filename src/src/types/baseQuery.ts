import { ThunkDispatch } from "redux-thunk"

export interface BaseQueryApi {
    signal: AbortSignal
    abort: (reason?: string) => void
    dispatch: ThunkDispatch<any, any, any>
    getState: () => unknown
    extra: unknown
    endpoint: string
    type: 'query' | 'mutation'
    forced?: boolean
  }