
import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Ciudad, GuardarCiudad } from '../../core/models/ciudad.model';
import { Departamento } from '../../core/models/departamento.model';
import { Pais } from '../../core/models/pais.model';

import { CiudadService } from '../../core/services/ciudad.service';
import { DepartamentoService } from '../../core/services/departamento.service';
import { PaisService } from '../../core/services/pais.service';

@Component({
  selector: 'app-ciudades',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ciudades.html',
  styleUrl: './ciudades.css'
})
export class Ciudades implements OnInit {
  private readonly ciudadService = inject(CiudadService);
  private readonly departamentoService = inject(DepartamentoService);
  private readonly paisService = inject(PaisService);
  private readonly cdr = inject(ChangeDetectorRef);

  ciudades: Ciudad[] = [];
  departamentos: Departamento[] = [];
  paises: Pais[] = [];

  formulario: GuardarCiudad = {
    departamentoId: 0,
    nombre: '',
    activo: true
  };

  ciudadEditando: number | null = null;
  cargando = false;
  guardando = false;
  mensaje = '';
  tipoMensaje: 'exito' | 'error' = 'exito';

  ngOnInit(): void {
    this.cargarPaises();
    this.cargarDepartamentos();
    this.cargarCiudades();
  }

  cargarPaises(): void {
    this.paisService.obtenerTodos().subscribe({
      next: (paises) => {
        this.paises = paises;
        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje('No fue posible cargar los países.', 'error');
        this.cdr.markForCheck();
      }
    });
  }

  cargarDepartamentos(): void {
    this.departamentoService.obtenerTodos().subscribe({
      next: (departamentos) => {
        this.departamentos = departamentos;
        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje('No fue posible cargar los departamentos.', 'error');
        this.cdr.markForCheck();
      }
    });
  }

  cargarCiudades(): void {
    this.cargando = true;

    this.ciudadService.obtenerTodos().subscribe({
      next: (ciudades) => {
        this.ciudades = ciudades;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje('No fue posible cargar las ciudades.', 'error');
        this.cargando = false;
        this.cdr.markForCheck();
      }
    });
  }

  guardar(): void {
    const nombre = this.formulario.nombre.trim();
    const activo = this.formulario.activo;

    if (!this.formulario.departamentoId) {
      this.mostrarMensaje('Debes seleccionar un departamento.', 'error');
      this.cdr.markForCheck();
      return;
    }

    if (!nombre) {
      this.mostrarMensaje('El nombre de la ciudad es obligatorio.', 'error');
      this.cdr.markForCheck();
      return;
    }

    if (nombre.length > 100) {
      this.mostrarMensaje('El nombre no puede superar los 100 caracteres.', 'error');
      this.cdr.markForCheck();
      return;
    }

    const datos: GuardarCiudad = {
      departamentoId: this.formulario.departamentoId,
      nombre,
      activo
    };

    const estabaEditando = this.ciudadEditando !== null;

    const peticion = estabaEditando
      ? this.ciudadService.actualizar(this.ciudadEditando!, datos)
      : this.ciudadService.crear(datos);

    this.guardando = true;
    this.mensaje = '';

    peticion.subscribe({
      next: () => {
        this.mostrarMensaje(
          estabaEditando
            ? 'Ciudad actualizada correctamente.'
            : 'Ciudad creada correctamente.',
          'exito'
        );

        this.limpiarFormulario();
        this.cargarCiudades();
        this.guardando = false;
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje = error.error?.mensaje
          ?? error.error?.detail
          ?? error.error?.title
          ?? 'No fue posible guardar la ciudad.';

        this.mostrarMensaje(mensaje, 'error');
        this.guardando = false;
        this.cdr.markForCheck();
      }
    });
  }

  editar(ciudad: Ciudad): void {
    this.ciudadEditando = ciudad.ciudadId;

    this.formulario = {
      departamentoId: ciudad.departamentoId,
      nombre: ciudad.nombre,
      activo: ciudad.activo
    };

    this.mensaje = '';
    this.cdr.markForCheck();
  }

  eliminar(ciudad: Ciudad): void {
    const confirmar = window.confirm(
      `¿Deseas eliminar la ciudad "${ciudad.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.ciudadService.eliminar(ciudad.ciudadId).subscribe({
      next: () => {
        this.mostrarMensaje('Ciudad eliminada correctamente.', 'exito');
        this.cargarCiudades();
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje = error.error?.mensaje
          ?? error.error?.detail
          ?? error.error?.title
          ?? 'No fue posible eliminar la ciudad.';

        this.mostrarMensaje(mensaje, 'error');
        this.cdr.markForCheck();
      }
    });
  }

  limpiarFormulario(): void {
    this.ciudadEditando = null;

    this.formulario = {
      departamentoId: 0,
      nombre: '',
      activo: true
    };

    this.mensaje = '';
    this.cdr.markForCheck();
  }

  obtenerNombreDepartamento(departamentoId: number): string {
    return this.departamentos.find(
      departamento => departamento.departamentoId === departamentoId
    )?.nombre ?? 'Departamento no encontrado';
  }

  obtenerNombrePaisPorDepartamento(departamentoId: number): string {
    const departamento = this.departamentos.find(
      item => item.departamentoId === departamentoId
    );

    if (!departamento) {
      return 'País no encontrado';
    }

    return this.paises.find(
      pais => pais.paisId === departamento.paisId
    )?.nombre ?? 'País no encontrado';
  }

  private mostrarMensaje(
    mensaje: string,
    tipo: 'exito' | 'error'
  ): void {
    this.mensaje = mensaje;
    this.tipoMensaje = tipo;
  }
}
