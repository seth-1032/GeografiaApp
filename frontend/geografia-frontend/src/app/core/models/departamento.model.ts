
export interface Departamento {
  departamentoId: number;
  paisId: number;
  nombre: string;
  activo: boolean;
}

export interface GuardarDepartamento {
  paisId: number;
  nombre: string;
  activo: boolean;
}
