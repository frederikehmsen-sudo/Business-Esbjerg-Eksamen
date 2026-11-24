/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/** @format int32 */
export enum Role {
  Value0 = 0,
  Value1 = 1,
  Value2 = 2,
}

/** @format int32 */
export enum RequestUnit {
  Value0 = 0,
}

export interface CreateHolidayRuleSetRequest {
  /**
   * @minLength 3
   * @maxLength 100
   */
  name: string;
  /** @format date */
  effectiveFrom: string;
  /**
   * @format double
   * @min 0.01
   * @max 10
   */
  weeks1To5DaysEarnedPerMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  weeks1To5EarningStartMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 36
   */
  weeks1To5EarningLengthMonths?: number;
  /**
   * @format int32
   * @min 0
   * @max 36
   */
  weeks1To5UsageStartsMonthsAfterEarningStart?: number;
  /**
   * @format int32
   * @min 1
   * @max 36
   */
  weeks1To5UsageLengthMonths?: number;
  week6Enabled?: boolean;
  /**
   * @format double
   * @min 0
   * @max 40
   */
  week6HoursEarnedPerMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  week6EarningStartMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 36
   */
  week6EarningLengthMonths?: number;
  /**
   * @format int32
   * @min 0
   * @max 36
   */
  week6UsageStartsMonthsAfterEarningStart?: number;
  /**
   * @format int32
   * @min 1
   * @max 36
   */
  week6UsageLengthMonths?: number;
  /**
   * @format int32
   * @min 1
   * @max 30
   */
  mainHolidayDays?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  mainHolidayWindowStartMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  mainHolidayWindowEndMonth?: number;
  /**
   * @format int32
   * @min 0
   * @max 12
   */
  noticeMainHolidayMonths?: number;
  /**
   * @format int32
   * @min 0
   * @max 12
   */
  noticeOtherHolidayMonths?: number;
  /**
   * @format int32
   * @min 0
   * @max 30
   */
  maxTransferDays?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  weeks1To5TransferDeadlineMonth?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  week6TransferDeadlineMonth?: number;
  /**
   * @format int32
   * @min 0
   * @max 30
   */
  maxHolidayDebtDays?: number;
  /**
   * @format int32
   * @min 1
   * @max 12
   */
  debtClearedByMonth?: number;
  requestUnit?: RequestUnit;
}

export interface CreateUserRequest {
  /**
   * @minLength 3
   * @maxLength 50
   * @pattern ^[a-zA-Z0-9._-]+$
   */
  username: string;
  /**
   * @minLength 8
   * @maxLength 128
   */
  password: string;
  role: Role;
}

export interface HolidayRuleSetResponse {
  /** @format uuid */
  id?: string;
  name?: string | null;
  /** @format date */
  effectiveFrom?: string;
  /** @format date-time */
  createdAt?: string;
  /** @format uuid */
  createdBy?: string;
  /** @format double */
  weeks1To5DaysEarnedPerMonth?: number;
  /** @format int32 */
  weeks1To5EarningStartMonth?: number;
  /** @format int32 */
  weeks1To5EarningLengthMonths?: number;
  /** @format int32 */
  weeks1To5UsageStartsMonthsAfterEarningStart?: number;
  /** @format int32 */
  weeks1To5UsageLengthMonths?: number;
  week6Enabled?: boolean;
  /** @format double */
  week6HoursEarnedPerMonth?: number;
  /** @format int32 */
  week6EarningStartMonth?: number;
  /** @format int32 */
  week6EarningLengthMonths?: number;
  /** @format int32 */
  week6UsageStartsMonthsAfterEarningStart?: number;
  /** @format int32 */
  week6UsageLengthMonths?: number;
  /** @format int32 */
  mainHolidayDays?: number;
  /** @format int32 */
  mainHolidayWindowStartMonth?: number;
  /** @format int32 */
  mainHolidayWindowEndMonth?: number;
  /** @format int32 */
  noticeMainHolidayMonths?: number;
  /** @format int32 */
  noticeOtherHolidayMonths?: number;
  /** @format int32 */
  maxTransferDays?: number;
  /** @format int32 */
  weeks1To5TransferDeadlineMonth?: number;
  /** @format int32 */
  week6TransferDeadlineMonth?: number;
  /** @format int32 */
  maxHolidayDebtDays?: number;
  /** @format int32 */
  debtClearedByMonth?: number;
  requestUnit?: string | null;
}

export interface LoginRequest {
  /**
   * @minLength 1
   * @maxLength 50
   */
  username: string;
  /**
   * @minLength 1
   * @maxLength 128
   */
  password: string;
}

export interface UpdateUserRequest {
  /**
   * @minLength 3
   * @maxLength 50
   * @pattern ^[a-zA-Z0-9._-]+$
   */
  username: string;
  /**
   * @minLength 8
   * @maxLength 128
   */
  password?: string | null;
  role?: Role;
}

export interface UserResponse {
  /** @format uuid */
  id?: string;
  username?: string | null;
  role?: string | null;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown>
  extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  JsonApi = "application/vnd.api+json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) =>
    fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter(
      (key) => "undefined" !== typeof query[key],
    );
    return keys
      .map((key) =>
        Array.isArray(query[key])
          ? this.addArrayQueryParam(query, key)
          : this.addQueryParam(query, key),
      )
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.JsonApi]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string")
        ? JSON.stringify(input)
        : input,
    [ContentType.Text]: (input: any) =>
      input !== null && typeof input !== "string"
        ? JSON.stringify(input)
        : input,
    [ContentType.FormData]: (input: any) => {
      if (input instanceof FormData) {
        return input;
      }

      return Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
              ? JSON.stringify(property)
              : `${property}`,
        );
        return formData;
      }, new FormData());
    },
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(
    params1: RequestParams,
    params2?: RequestParams,
  ): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (
    cancelToken: CancelToken,
  ): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(
      `${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`,
      {
        ...requestParams,
        headers: {
          ...(requestParams.headers || {}),
          ...(type && type !== ContentType.FormData
            ? { "Content-Type": type }
            : {}),
        },
        signal:
          (cancelToken
            ? this.createAbortSignal(cancelToken)
            : requestParams.signal) || null,
        body:
          typeof body === "undefined" || body === null
            ? null
            : payloadFormatter(body),
      },
    ).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const responseToParse = responseFormat ? response.clone() : response;
      const data = !responseFormat
        ? r
        : await responseToParse[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title API
 * @version 1.0
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags HolidayRuleSet
     * @name HolidayRuleSetList
     * @request GET:/api/HolidayRuleSet
     * @secure
     */
    holidayRuleSetList: (params: RequestParams = {}) =>
      this.request<HolidayRuleSetResponse[], any>({
        path: `/api/HolidayRuleSet`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags HolidayRuleSet
     * @name HolidayRuleSetCreate
     * @request POST:/api/HolidayRuleSet
     * @secure
     */
    holidayRuleSetCreate: (
      data: CreateHolidayRuleSetRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/HolidayRuleSet`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags HolidayRuleSet
     * @name HolidayRuleSetInForceList
     * @request GET:/api/HolidayRuleSet/in-force
     * @secure
     */
    holidayRuleSetInForceList: (
      query?: {
        /** @format date */
        date?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/HolidayRuleSet/in-force`,
        method: "GET",
        query: query,
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags HolidayRuleSet
     * @name HolidayRuleSetPresetsList
     * @request GET:/api/HolidayRuleSet/presets
     * @secure
     */
    holidayRuleSetPresetsList: (params: RequestParams = {}) =>
      this.request<CreateHolidayRuleSetRequest[], any>({
        path: `/api/HolidayRuleSet/presets`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserList
     * @request GET:/api/User
     * @secure
     */
    userList: (params: RequestParams = {}) =>
      this.request<UserResponse[], any>({
        path: `/api/User`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserCreate
     * @request POST:/api/User
     * @secure
     */
    userCreate: (data: CreateUserRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/User`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserDelete
     * @request DELETE:/api/User/{id}
     * @secure
     */
    userDelete: (id: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/User/${id}`,
        method: "DELETE",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserUpdate
     * @request PUT:/api/User/{id}
     * @secure
     */
    userUpdate: (
      id: string,
      data: UpdateUserRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/User/${id}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),
  };
  login = {
    /**
     * No description
     *
     * @tags Login
     * @name LoginCreate
     * @request POST:/login
     * @secure
     */
    loginCreate: (data: LoginRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/login`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),
  };
}
