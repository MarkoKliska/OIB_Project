import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { VehicleService } from '../../../shared/services/vehicle.service';
import { InvoiceService } from '../../../shared/services/invoice.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { VehicleResponseDto } from '../../../shared/dto/responses/vehicle.request.dto';
import { ServiceInvoiceResponseDto } from '../../../shared/dto/responses/invoice.response.dto';

type ActiveTab = 'vehicles' | 'add-vehicle' | 'invoices';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule],
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.scss']
})
export class ManagerDashboardComponent implements OnInit {
  activeTab: ActiveTab = 'vehicles';
  fullName: string | null = '';

  vehicles: VehicleResponseDto[] = [];
  invoices: ServiceInvoiceResponseDto[] = [];

  isLoadingVehicles = false;
  isLoadingInvoices = false;
  isSubmitting = false;

  vehicleTypes = ['Passenger', 'Truck', 'Motorcycle'];
  vehicleTypeLabels: { [key: string]: string | undefined } = {
    Passenger: 'Putničko',
    Truck: 'Teretno',
    Motorcycle: 'Motocikl'
  };

  addVehicleForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private invoiceService: InvoiceService,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.addVehicleForm = this.fb.group({
      licensePlate: ['', [Validators.required, Validators.minLength(2)]],
      brand:        ['', [Validators.required, Validators.minLength(2)]],
      model:        ['', [Validators.required, Validators.minLength(1)]],
      type:         ['Passenger', Validators.required],
      estimatedPrice: [null, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.fullName = this.authService.getFullName();
    this.loadVehicles();
  }

  // ==================== TABS ====================

  setTab(tab: ActiveTab): void {
    this.activeTab = tab;
    if (tab === 'vehicles' && this.vehicles.length === 0) this.loadVehicles();
    if (tab === 'invoices' && this.invoices.length === 0) this.loadInvoices();
  }

  // ==================== VEHICLES ====================

  loadVehicles(): void {
    this.isLoadingVehicles = true;
    this.vehicleService.getAllVehicles().subscribe({
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

  get servicedCount(): number {
    return this.vehicles.filter(v => v.isServiced).length;
  }

  get pendingCount(): number {
    return this.vehicles.filter(v => !v.isServiced).length;
  }

  // ==================== ADD VEHICLE ====================

  get lp()    { return this.addVehicleForm.get('licensePlate')!; }
  get brand() { return this.addVehicleForm.get('brand')!; }
  get model() { return this.addVehicleForm.get('model')!; }
  get type()  { return this.addVehicleForm.get('type')!; }
  get price() { return this.addVehicleForm.get('estimatedPrice')!; }

  onAddVehicle(): void {
    if (this.addVehicleForm.invalid) {
      this.addVehicleForm.markAllAsTouched();
      return;
    }
    this.isSubmitting = true;
    this.vehicleService.addVehicle(this.addVehicleForm.value).subscribe({
      next: (v) => {
        this.toastService.success(`Vozilo ${v.brand} ${v.model} uspešno dodato!`);
        this.addVehicleForm.reset({ type: 'Passenger' });
        this.vehicles = [];
        this.setTab('vehicles');
        this.isSubmitting = false;
      },
      error: (err) => {
        const msg = err?.error?.error ?? 'Greška pri dodavanju vozila.';
        this.toastService.error(msg);
        this.isSubmitting = false;
      }
    });
  }

  // ==================== INVOICES ====================

  loadInvoices(): void {
    this.isLoadingInvoices = true;
    this.invoiceService.getAllInvoices().subscribe({
      next: (data) => {
        this.invoices = data;
        this.isLoadingInvoices = false;
      },
      error: () => {
        this.toastService.error('Greška pri učitavanju računa.');
        this.isLoadingInvoices = false;
      }
    });
  }

  // ==================== AUTH ====================

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}