import type { VoucherStatus } from "@/enums/voucherStatus"

export interface Voucher {
  id: number
  code: string
  originalValue: number
  currentValue: number
  createdAt: string
  updatedAt: string
  status: VoucherStatus
}