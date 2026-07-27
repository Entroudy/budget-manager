export default defineNuxtPlugin(() => {
  function getCurrentDateString() {
    var currentDate = new Date()

    var day = currentDate.getDate()
    var month = currentDate.getMonth() + 1
    var year = currentDate.getFullYear()

    var dayStr = day < 10 ? '0' + day : '' + day
    var monthStr = month < 10 ? '0' + month : '' + month

    return monthStr + '/' + dayStr + '/' + year
  }

  const priceFormatter = new Intl.NumberFormat('hu-HU', {
    style: 'decimal',
    useGrouping: true,
  })
  return {
    provide: {
      removeItemsFromArray(selectedItems: any, source: any) {
        const data = [...source]
        for (let i = selectedItems.length - 1; i >= 0; i--) {
          const identifier = selectedItems[i]
          if (typeof identifier === 'string') {
            const index = data.findIndex((item) => item.id === identifier)
            if (index !== -1) {
              data.splice(index, 1)
            }
          } else if (typeof identifier === 'number') {
            if (identifier >= 0 && identifier < data.length) {
              data.splice(identifier, 1)
            }
          }
        }
        return data
      },
      formatDateString(dateStr: string) {
        const date = new Date(dateStr)

        const month = String(date.getUTCMonth() + 1).padStart(2, '0')
        const day = String(date.getUTCDate()).padStart(2, '0')

        return `${month}.${day}`
      },
      toLowerCase(text: string): string {
        return text.toLocaleLowerCase()
      },
      formatHours(value: number) {
        return value >= 0 && value ? value + 'h' : '-'
      },
      shortDateInHU(date: string): string {
        const dateStr = new Date(date).toLocaleDateString('hu-HU', {
          year: 'numeric',
          month: 'numeric',
          day: 'numeric',
        })
        return dateStr
      },
      shortDateTimeInHU(date: string): string {
        const dateStr = new Date(date).toLocaleTimeString('hu-HU', {
          year: 'numeric',
          month: 'numeric',
          day: 'numeric',
          hour: 'numeric',
          minute: 'numeric',
        })
        return dateStr
      },
      isDateInRange(startDateStr: string, endDateStr: string): boolean {
        var userDate = new Date()
        var startDate = new Date(startDateStr)
        var endDate = new Date(endDateStr)

        return startDate <= userDate && userDate <= endDate
      },
      copyWorkItemLinkToClipboard(workItemId: string) {
        navigator.clipboard.writeText(`${window.location.origin}/?work-item=${workItemId}`)
      },
      formatPrice: (value) => `${priceFormatter.format(value)} Ft`,
      checkDateDifference(inputDateString: any) {
        const inputDate = new Date(inputDateString)
        const currentDate = new Date()

        const differenceInMilliseconds = currentDate.getTime() - inputDate.getTime()

        const differenceInDays = differenceInMilliseconds / (1000 * 60 * 60 * 24)

        if (differenceInDays > 56) {
          return 'bg-error'
        } else if (differenceInDays > 42) {
          return 'bg-warning'
        }
      },
      calculatePercentage(part, whole) {
        let result = (part / whole) * 100 || 0
        result = result === Infinity ? 0 : result
        return `${result.toFixed(2)}%`
      },
      getXDayOfMonth(x_date: number) {
        const today = new Date()
        return new Date(today.getFullYear(), today.getMonth(), x_date)
      },
      getFirstDayOfYear() {
        const today = new Date()
        const firstDay = new Date(today.getFullYear(), 0, 1)

        return firstDay
      },
      getLastDayOfMonth() {
        const today = new Date()
        const lastDay = new Date(today.getFullYear(), today.getMonth() + 1, 0)

        return lastDay.getDate()
      },
      getNextMonthFirstDay() {
        const today = new Date()
        const nextMonthFirstDay = new Date(today.getFullYear(), today.getMonth() + 1, 1)
        return nextMonthFirstDay
      },
      formatValuePercentage(value: number) {
        if (value % 1 >= 0.5) {
          return `${Math.ceil(value)}%`
        } else {
          return `${Math.floor(value)}%`
        }
      },
      formatNumber(input: number | string) {
        if (input) {
          let number = typeof input === 'string' ? parseInt(input.replace(/\s+/g, ''), 10) : input
          if (isNaN(number)) {
            throw new Error('Invalid input. Please provide a valid number or numeric string.')
          }

          return number.toLocaleString('en-US').replace(/,/g, ' ')
        }
        return input
      },
      formatDatePickerValue(modelData) {
        if (modelData && modelData instanceof Date && !isNaN(modelData)) {
          modelData.setHours(0, 0, 0, 0);
          const year = modelData.getFullYear();
          const month = (modelData.getMonth() + 1).toString().padStart(2, '0');
          const day = modelData.getDate().toString().padStart(2, '0');
          const hours = modelData.getHours().toString().padStart(2, '0');
          const minutes = modelData.getMinutes().toString().padStart(2, '0');
          const seconds = modelData.getSeconds().toString().padStart(2, '0');

          return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
        } else {
          return null;
        }
      }
    },
  }
})
