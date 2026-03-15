export interface AddVehicleRequestDto {
  licensePlate: string;
  brand: string;
  model: string;
  type: string; // 'Passenger' | 'Truck' | 'Motorcycle'
  estimatedPrice: number;
}