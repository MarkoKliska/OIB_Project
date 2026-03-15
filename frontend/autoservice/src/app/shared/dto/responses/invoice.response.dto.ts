export interface ServiceInvoiceResponseDto {
  id: string;
  mechanicName: string;
  issuedAt: string; 
  totalAmount: number;
  vehicleLicensePlate: string;
  vehicleBrand: string;
  vehicleModel: string;
}