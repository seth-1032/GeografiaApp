
import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Departamento, GuardarDepartamento } from '../../core/models/departamento.model';
import { Pais } from '../../core/models/pais.model';
import { DepartamentoService } from '../../core/services/departamento.service';
import { PaisService } from '../../core/services/pais.service';

@Component({
  selector: 'app-departamentos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './departamentos.html',
  styleUrl: './departamentos.css'
})
export class Departamentos implements OnInit {
  private readonly departamentoService = inject(DepartamentoService);
  private readonly paisService = inject(PaisService);
  private readonly cdr = inject(ChangeDetectorRef);

  departamentos: Departamento[] = [];
  paises: Pais[] = [];

  formulario: GuardarDepartamento = {
    paisId: 0,
    nombre: '',
    activo: true
  };

  departamentoEditando: number | null = null;
  cargando = false;
  guardando = false;
  mensaje = '';
  tipoMensaje: 'exito' | 'error' = 'exito';

  ngOnInit(): void {
    this.cargarPaises();
    this.cargarDepartamentos();
  }

  cargarPaises(): void {
    this.paisService.obtenerTodos().subscribe({
      next: (paises) => {
        this.paises = paises.filter(pais => pais.activo);
        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje('No fue posible cargar los países.', 'error');
        this.cdr.markForCheck();
      }
    });
  }

  cargarDepartamentos(): void {
    this.cargando = true;

    this.departamentoService.obtenerTodos().subscribe({
      next: (departamentos) => {
        this.departamentos = departamentos;
        this.cargando = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje('No fue posible cargar los departamentos.', 'error');
        this.cargando = false;
        this.cdr.markForCheck();
      }
    });
  }

  guardar(): void {
    const nombre = this.formulario.nombre.trim();
    const activo = this.formulario.activo;

    if (!this.formulario.paisId) {
      this.mostrarMensaje('Debes seleccionar un país.', 'error');
      this.cdr.markForCheck();
      return;
    }

    if (!nombre) {
      this.mostrarMensaje('El nombre del departamento es obligatorio.', 'error');
      this.cdr.markForCheck();
      return;
    }

    if (nombre.length > 100) {
      this.mostrarMensaje('El nombre no puede superar los 100 caracteres.', 'error');
      this.cdr.markForCheck();
      return;
    }

    const datos: GuardarDepartamento = {
      paisId: this.formulario.paisId,
      nombre,
      activo
    };

    this.guardando = true;
    this.mensaje = '';

    const peticion = this.departamentoEditando === null
      ? this.departamentoService.crear(datos)
      : this.departamentoService.actualizar(this.departamentoEditando, datos);

    peticion.subscribe({
      next: () => {
        this.mostrarMensaje(
          this.departamentoEditando === null
            ? 'Departamento creado correctamente.'
            : 'Departamento actualizado correctamente.',
          'exito'
        );

        this.limpiarFormulario();
        this.cargarDepartamentos();
        this.guardando = false;
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje = error.error?.mensaje
          ?? error.error?.detail
          ?? error.error?.title
          ?? 'No fue posible guardar el departamento.';

        this.mostrarMensaje(mensaje, 'error');
        this.guardando = false;
        this.cdr.markForCheck();
      }
    });
  }

  editar(departamento: Departamento): void {
    this.departamentoEditando = departamento.departamentoId;

    this.formulario = {
      paisId: departamento.paisId,
      nombre: departamento.nombre,
      activo: departamento.activo
    };

    this.mensaje = '';
    this.cdr.markForCheck();
  }

  eliminar(departamento: Departamento): void {
    const confirmar = window.confirm(
      `¿Deseas eliminar el departamento "${departamento.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.departamentoService.eliminar(departamento.departamentoId).subscribe({
      next: () => {
        this.mostrarMensaje('Departamento eliminado correctamente.', 'exito');
        this.cargarDepartamentos();
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje = error.error?.mensaje
          ?? error.error?.detail
          ?? error.error?.title
          ?? 'No fue posible eliminar el departamento. Puede tener ciudades asociadas.';

        this.mostrarMensaje(mensaje, 'error');
        this.cdr.markForCheck();
      }
    });
  }

  limpiarFormulario(): void {
    this.departamentoEditando = null;

    this.formulario = {
      paisId: 0,
      nombre: '',
      activo: true
    };

    this.mensaje = '';
    this.cdr.markForCheck();
  }

  obtenerNombrePais(paisId: number): string {
    return this.paises.find(pais => pais.paisId === paisId)?.nombre
      ?? 'País no encontrado';
  }

  private mostrarMensaje(
    mensaje: string,
    tipo: 'exito' | 'error'
  ): void {
    this.mensaje = mensaje;
    this.tipoMensaje = tipo;
  }
}
