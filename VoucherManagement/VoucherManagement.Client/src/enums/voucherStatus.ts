export type VoucherStatus =
  | 'Created'
  | 'Printed'
  | 'Issued'
  | 'PartiallyRedeemed'
  | 'FullyRedeemed'
  | 'Cancelled'

export const VoucherStatusLabel: Record<VoucherStatus, string> = {
  Created: 'Erstellt',
  Printed: 'Gedruckt',
  Issued: 'Ausgegeben',
  PartiallyRedeemed: 'Teilweise eingelöst',
  FullyRedeemed: 'Vollständig eingelöst',
  Cancelled: 'Storniert'
}