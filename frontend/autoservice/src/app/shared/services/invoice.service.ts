import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { ServiceInvoiceResponseDto } from '../dto/responses/invoice.response.dto';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private readonly api = `${environment.apiUrl}/invoices`;

  constructor(private http: HttpClient) {}

  // Manager: get all invoices
  getAllInvoices(): Observable<ServiceInvoiceResponseDto[]> {
    return this.http.get<ServiceInvoiceResponseDto[]>(this.api);
  }

  // Mechanic: complete service and issue invoice
  completeService(vehicleId: string): Observable<ServiceInvoiceResponseDto> {
    return this.http.post<ServiceInvoiceResponseDto>(`${this.api}/complete/${vehicleId}`, {});
  }
}