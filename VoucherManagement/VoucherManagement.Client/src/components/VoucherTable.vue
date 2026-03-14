<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { useVoucherStore } from '@/stores/voucherStore';
import { onMounted, ref } from 'vue';
import { formatDateCET } from '@/lib/date';
import type { VoucherStatus } from "@/enums/voucherStatus"
import { SquarePlusIcon, SearchIcon } from 'lucide-vue-next'

const voucherStore = useVoucherStore()
const isDialogOpen = ref(false)
const searchCode = ref('')
const selectedVoucher = ref<any>(null)
const editedVoucher = ref({
  code: '',
  originalValue: 0,
  currentValue: 0,
  status: "Created" as VoucherStatus
})

onMounted(async() => {
  await voucherStore.fetchAll()
})

const searchVoucher = () => {
  const voucher = voucherStore.vouchers.find(
    v => v.code.toLowerCase() === searchCode.value.trim().toLowerCase()
  )

  if (voucher) {
    openDialog(voucher)
  } else {
    alert('Gutschein nicht gefunden')
  }
}

const openDialog = (voucher: any) => {
  selectedVoucher.value = voucher
  editedVoucher.value = {
    code: voucher.code,
    currentValue: voucher.currentValue,
    originalValue: voucher.originalValue,
    status: voucher.status
  }
  isDialogOpen.value = true
}

const closeDialog = () => {
  isDialogOpen.value = false
  selectedVoucher.value = null
}

const saveVoucher = async () => {
  if (selectedVoucher.value) {
    // Hier deine Store-Methode zum Aktualisieren aufrufen
    // await voucherStore.updateVoucher(selectedVoucher.value.id, editedVoucher.value)
    await voucherStore.update(selectedVoucher.value.id, editedVoucher.value) // Aktualisiere die Liste nach dem Speichern
    console.log('Speichere Gutschein:', selectedVoucher.value.id, editedVoucher.value)
    await voucherStore.fetchAll()
    closeDialog()
  }
}

const openPdf = async (voucherId: number) => {
  await voucherStore.openPdf(voucherId)
  await voucherStore.fetchAll()
  closeDialog()
}

const createVoucher = async () => {
  await voucherStore.create()
  await voucherStore.fetchAll()
}

</script>

<template>
  <div>
     <div class="flex w-full max-w-xl items-center space-x-2">
      <Input type="text" placeholder="Gutschein Code" v-model="searchCode" @keyup.enter="searchVoucher"/>
      <Button type="button" @click="searchVoucher"><SearchIcon/> Suche</Button>
      <Button @click="createVoucher"><SquarePlusIcon /> Gutschein erstellen</Button>
     </div>
    <div class="pt-4">
      <Table class="pt-4">
        <TableCaption>Alle Gutscheine</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead class="w-[100px]">ID</TableHead>
            <TableHead>Erstellt am</TableHead>
            <TableHead>Code</TableHead>
            <TableHead class="text-right">Guthaben Start</TableHead>
            <TableHead class="text-right">Guthaben Aktuell</TableHead>
            <TableHead>Aktualisiert am</TableHead>
            <TableHead>Status</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow 
            v-for="voucher in voucherStore.vouchers" 
            :key="voucher.id"
            @click="openDialog(voucher)"
            class="cursor-pointer hover:bg-muted/50"
          >
            <TableCell class="font-medium">{{ voucher.id }}</TableCell>
            <TableCell>{{ formatDateCET(voucher.createdAt) }}</TableCell>
            <TableCell>{{ voucher.code }}</TableCell>
            <TableCell class="text-right">{{ voucher.originalValue }} €</TableCell>
            <TableCell class="text-right">{{ voucher.currentValue }} €</TableCell>
            <TableCell>{{ formatDateCET(voucher.updatedAt) }}</TableCell>
            <TableCell>{{ voucher.status }}</TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>

    <Dialog v-model:open="isDialogOpen">
      <DialogContent class="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Gutschein bearbeiten</DialogTitle>
          <DialogDescription>
            Gutschein {{ selectedVoucher?.code }} bearbeiten
          </DialogDescription>
        </DialogHeader>
        <div class="grid gap-4 py-4">
          <div class="grid grid-cols-4 items-center gap-4">
            <Label for="code" class="text-right">
              Code
            </Label>
            <Input
              id="code"
              v-model="editedVoucher.code"
              class="col-span-3"
              readonly
            />
          </div>
          <div class="grid grid-cols-4 items-center gap-4">
            <Label for="originalValue" class="text-right">
              Guthaben Start
            </Label>
            <Input
              id="originalValue"
              v-model.number="editedVoucher.originalValue"
              type="number"
              class="col-span-3"
              :disabled="editedVoucher.status !== 'Printed'"
            />
          </div>
          <div class="grid grid-cols-4 items-center gap-4">
            <Label for="currentValue" class="text-right">
              Guthaben aktuell
            </Label>
            <Input
              id="currentValue"
              v-model.number="editedVoucher.currentValue"
              type="number"
              class="col-span-3"
              :disabled="editedVoucher.status == 'Created' || editedVoucher.status == 'Printed' || editedVoucher.status == 'FullyRedeemed'"
            />
          </div>
          <div class="grid grid-cols-4 items-center gap-4">
            <Label for="status" class="text-right">
              Status
            </Label>
            <Input
              id="status"
              v-model="editedVoucher.status"
              class="col-span-3"
              disabled
            />
          </div>
        </div>
        <DialogFooter>
          <Button type="button" variant="outline" @click="closeDialog">
            Abbrechen
          </Button>
          <Button type="submit" @click="saveVoucher">
            Speichern
          </Button>
         <Button v-if="editedVoucher.status == 'Created'" type="submit" @click="openPdf(selectedVoucher.id)">
            Drucken
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  </div>
</template>