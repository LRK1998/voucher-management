import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/lib/api'
import type { Voucher } from '@/types/voucher'

export const useVoucherStore = defineStore('vouchers', () => {
  // state
  const vouchers = ref<Voucher[]>([])
  const isLoading = ref(false)

  // getters
  const getAll = computed(() => vouchers.value)

  const getById = (id: number) =>
    vouchers.value.find(n => n.id === id)

  // actions
  async function fetchAll() {
    isLoading.value = true
    try {
      // Beispiel: API Call
      const res = await api.get<Voucher[]>('/vouchers')
      vouchers.value = res.data
    } finally {
      isLoading.value = false
    }
  }

  async function create() {
    isLoading.value = true
    try {
      const res = await api.post<Voucher>('/vouchers')

      // Backend liefert den neu erstellten Voucher zurück
      vouchers.value.push(res.data)

      return res.data
    } finally {
      isLoading.value = false
    }
  }

  async function update(id: number, patch: Partial<Omit<Voucher, 'id'>>) {
    isLoading.value = true
    try {
      await api.put(`/vouchers/${id}`, patch)

      const index = vouchers.value.findIndex(v => v.id === id)
      if (index === -1) return null

      vouchers.value[index] = {
        ...vouchers.value[index],
        ...patch,
        updatedAt: new Date().toISOString()
      } as Voucher

      return vouchers.value[index]
    } finally {
      isLoading.value = false
    }
  }

async function openPdf(id: number) {
  isLoading.value = true
  try {
    const res = await api.get(`/vouchers/${id}/pdf`, {
      responseType: 'blob'
    })

    const blob = new Blob([res.data], { type: 'application/pdf' })
    const url = window.URL.createObjectURL(blob)

    window.open(url, '_blank')

    // optional: später freigeben
    // setTimeout(() => URL.revokeObjectURL(url), 1000)
  } finally {
    isLoading.value = false
  }
}

  return {
    // state
    vouchers,
    isLoading,

    // getters
    getAll,
    getById,

    // actions
    fetchAll,
    create,
    update,
    openPdf
  }
})