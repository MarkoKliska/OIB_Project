import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { VehicleService } from '../../../shared/services/vehicle.service';
import { InvoiceService } from '../../../shared/services/invoice.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { VehicleResponseDto } from '../../../shared/dto/responses/vehicle.request.dto';
import { ServiceInvoiceResponseDto } from '../../../shared/dto/responses/invoice.response.dto';

type ActiveTab = 'vehicles' | 'complete';

@Component({
  selector: 'app-mechanic-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mechanic-dashboard.component.html',
  styleUrls: ['./mechanic-dashboard.component.scss']
})
export class MechanicDashboardComponent implements OnInit {
  activeTab: ActiveTab = 'vehicles';
  fullName: string | null = '';

  vehicles: VehicleResponseDto[] = [];
  lastIssuedInvoice: ServiceInvoiceResponseDto | null = null;

  isLoadingVehicles = false;
  completingVehicleId: string | null = null;

  vehicleTypeLabels: { [key: string]: string | undefined } = {
    Passenger: 'Putničko',
    Truck: 'Teretno',
    Motorcycle: 'Motocikl'
  };

  constructor(
    private vehicleService: VehicleService,
    private invoiceService: InvoiceService,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fullName = this.authService.getFullName();
    this.loadVehicles();
  }

  setTab(tab: ActiveTab): void {
    this.activeTab = tab;
  }

  // ==================== VEHICLES ====================

  loadVehicles(): void {
    this.isLoadingVehicles = true;
    this.vehicleService.getUnservicedVehicles().subscribe({
      next: (data) => {
        this.vehicles = data;
        this.isLoadingVehicles = false;
      },
      error: () => {
        this.toastService.error('Greška pri učitavanju vozila.');
        this.isLoadingVehicles = false;
      }
    });
  }

  // ==================== COMPLETE SERVICE ====================

  completeService(vehicleId: string): void {
    this.completingVehicleId = vehicleId;
    this.invoiceService.completeService(vehicleId).subscribe({
      next: (invoice) => {
        this.toastService.success(
          `Servis završen! Račun: ${invoice.totalAmount.toFixed(2)} RSD`
        );
        this.lastIssuedInvoice = invoice;
        this.vehicles = this.vehicles.filter(v => v.id !== vehicleId);
        this.completingVehicleId = null;
        this.setTab('complete');
      },
      error: (err) => {
        const msg = err?.error?.error ?? 'Greška pri završavanju servisa.';
        this.toastService.error(msg);
        this.completingVehicleId = null;
      }
    });
  }

  // ==================== AUTH ====================

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}