import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { VehicleResponseDto } from '../dto/responses/vehicle.request.dto';
import { AddVehicleRequestDto } from '../dto/requests/vehicle.request.dto';

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private readonly api = `${environment.apiUrl}/vehicles`;

  constructor(private http: HttpClient) {}

  // Manager: get all vehicles
  getAllVehicles(): Observable<VehicleResponseDto[]> {
    return this.http.get<VehicleResponseDto[]>(this.api);
  }

  // Manager: add a vehicle
  addVehicle(request: AddVehicleRequestDto): Observable<VehicleResponseDto> {
    return this.http.post<VehicleResponseDto>(this.api, request);
  }

  // Mechanic: get unserviced vehicles
  getUnservicedVehicles(): Observable<VehicleResponseDto[]> {
    return this.http.get<VehicleResponseDto[]>(`${this.api}/unserviced`);
  }
}